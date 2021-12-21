<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <v-row>
        <v-col>
          <h1>Aktivität anlegen</h1>
        </v-col>
        <v-col>
          <v-btn class="pageButton"
            v-if="step === 1 && !updateMode"
            @click="uploadGpx = true">GPX hochladen</v-btn>
        </v-col>
        <v-progress-linear indeterminate v-if="loading" />
      </v-row>
      <v-stepper v-model="step">
        <v-stepper-header>
          <v-stepper-step
            :complete="step > 1"
            step="1">
            Grunddaten
          </v-stepper-step>
          <v-stepper-step
            :complete="step > 2"
            step="2">
            Notizen
          </v-stepper-step>
          <v-stepper-step
            step="3">
            Abschließen
          </v-stepper-step>
        </v-stepper-header>
        <v-stepper-items>
          <v-stepper-content step="1">
            <v-row>
              <v-col style="margin-top: 10px">
                <v-form v-model="valid">
                  <v-text-field
                    v-model="title"
                    :rules="[v => !!v || 'Titel wird benötigt']"
                    label="Titel"
                    required
                    dense
                    outlined />

                  <summits-autocomplete
                    v-model="summits"
                    :disabled="updateMode"
                  />

                  <date-selector
                    v-model="hikeDate"
                    required
                    :rules="[v => !!v || 'Datum wird benötigt']"
                    label="Datum" />

                  <time-selector
                    prepend="mdi-timer"
                    v-model="startTime"
                    label="Startzeit" />
                  <time-selector
                    prepend="mdi-timer"
                    v-model="endTime"
                    label="Endzeit" />

                  <v-text-field
                    v-model="elevationUp"
                    :rules="elevationRules"
                    label="Höhenmeter aufwärts"
                    type="number"
                    suffix="m"
                    required
                    dense
                    outlined />
                  <v-text-field
                    v-model="elevationDown"
                    :rules="elevationRules"
                    label="Höhenmeter abwärts"
                    type="number"
                    suffix="m"
                    required
                    dense
                    outlined />
                  <v-text-field
                    v-model="distance"
                    :rules="distanceRules"
                    label="Distanz"
                    suffix="m"
                    type="number"
                    required
                    dense
                    outlined />

                  <v-btn color="success"
                    @click="step = 2"
                    :disabled="!valid">
                    Weiter
                  </v-btn>
                </v-form>
              </v-col>
              <v-col>
                <summits-map
                  ref="map"
                  :summits="summits"
                  :autoCenter="true"
                  :loading="false"
                  :polyline="polyline" />
              </v-col>
            </v-row>
          </v-stepper-content>
          <v-stepper-content step="2">
            <v-row style="margin-top: 10px;">
              <v-col>
                <v-textarea outlined dense
                  v-model="notes"
                  label="Notizen / Beschreibung der Tour" />
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-rating
                  style="margin-top: -20px;"
                  background-color="orange lighten-3"
                  color="orange"
                  large
                  v-model="rating" />
              </v-col>
            </v-row>
            <v-btn color="default"
              style="margin-top: 20px"
              @click="step = 1">
              Zurück
            </v-btn>
            <v-btn color="success"
              style="margin-top: 20px; margin-left: 10px;"
              @click="step = 3"
              :disabled="rating === 0">
              Weiter
            </v-btn>
          </v-stepper-content>
          <v-stepper-content step="3">
            <div v-if="wishlistItems.length > 0">
              <h4>Von Wunschliste streichen?</h4>
              <v-list>
                <v-list-item v-for="wishlistItem in wishlistItems" :key="wishlistItem.id">
                  <v-list-item-action>
                    <v-checkbox v-model="wishlistItem.removeFromWishlist" />
                  </v-list-item-action>
                  <v-list-item-content>
                    <v-list-item-title>
                      {{wishlistItem.summit.name}}
                    </v-list-item-title>
                  </v-list-item-content>
                </v-list-item>
              </v-list>
            </div>
            <v-btn color="default"
              style="margin-top: 20px"
              @click="step = 2">
              Zurück
            </v-btn>
            <v-btn color="success"
              style="margin-top: 20px; margin-left: 10px;"
              @click="saveActivity">
              Speichern
            </v-btn>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-sheet>
    <v-dialog
      width="500"
      v-model="uploadGpx">
      <v-card>
        <v-card-title>GPX hochladen</v-card-title>
        <v-card-text>
          <v-progress-linear indeterminate v-if="gpxLoading" class="margin-bottom: 10px" />
          <v-file-input v-model="gpxFile"
            counter
            label="GPX-Dateien auswählen"
            accept="application/gpx+xml"
            placeholder="GPX-Dateien auswählen"
            prepend-icon="mdi-paperclip"
            outlined />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="uploadGpx = false">Abbrechen</v-btn>
          <v-btn text @click="analyzeGpx"
            :disabled="gpxFile === null">Hochladen</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import { latLng } from 'leaflet';
import DateSelector from '../../components/Common/DateSelector.vue';
import TimeSelector from '../../components/Common/TimeSelector.vue';
import SummitsAutocomplete from '../../components/Summits/SummitsAutocomplete.vue';
import SummitsMap from '../../components/Summits/SummitsMap.vue';
import BackendService from '../../services/BackendService';

export default {
  components: {
    DateSelector,
    TimeSelector,
    SummitsAutocomplete,
    SummitsMap,
  },
  props: {
    activityId: String,
  },
  data: () => ({
    valid: true,
    gpxFile: null,
    title: '',
    elevationUp: 0,
    elevationDown: 0,
    distance: 0,
    summits: [],
    hikeDate: new Date().toISOString(),
    startTime: null,
    endTime: null,
    loading: false,
    uploadGpx: false,
    gpxLoading: false,
    polyline: [],
    step: 1,
    notes: '',
    rating: 0,
    wishlistItems: [],

    activityToUpdate: null,

    elevationRules: [
      (v) => !!v || 'Höhenmeter werden benötigt',
      (v) => (v && v > 0) || 'Höhenmeter müssen positiv sein',
    ],
    distanceRules: [
      (v) => !!v || 'Distanz wird benötigt',
      (v) => (v && v > 0) || 'Distanz müssen positiv sein',
    ],
  }),
  watch: {
    summits(summits) {
      this.getWishlistForSummits(summits);

      const bounds = summits.map((s) => latLng(s.latitude, s.longitude));

      this.$refs.map.fitBounds(bounds, [100, 100]);
    },
  },
  methods: {
    async saveActivity() {
      const activity = {
        title: this.title,
        hikeDate: this.hikeDate,
        elevationUp: this.elevationUp,
        elevationDown: this.elevationDown,
        distance: this.distance,
        startTime: this.startTime,
        endTime: this.endTime,
        rating: this.rating,
        notes: this.notes,
        summitIds: this.summits.map((s) => s.id),
      };
      this.loading = true;

      if (this.updateMode) {
        activity.id = this.activityToUpdate.id;

        const updatedActivity = await BackendService.updateActivity(activity);
        this.loading = false;

        this.$router.push(`/activities/${updatedActivity.id}`);
        return;
      }

      const createdActivity = await BackendService.createActivity(activity);
      this.uploadGpxFile(createdActivity.id);

      for (let i = 0; i < this.wishlistItems.length; i += 1) {
        const wishlistItem = this.wishlistItems[i];
        if (wishlistItem.removeFromWishlist) {
          await BackendService.finishWishlistItem(wishlistItem);
        }
      }

      this.loading = false;

      this.$router.push(`/activities/${createdActivity.id}`);
    },
    async uploadGpxFile(activityId) {
      if (this.gpxFile) {
        await BackendService.uploadGpx(activityId, this.gpxFile);
      }
    },
    async getWishlistForSummits(summits) {
      if (!summits || this.updateMode) return;
      const wishlistItems = [];
      for (let i = 0; i < summits.length; i += 1) {
        const summit = summits[i];
        try {
          const wishlist = await BackendService.getWishlistItemForSummit(summit.id);
          wishlist.removeFromWishlist = true;
          wishlistItems.push(wishlist);
        } catch (err) {
          //
        }
      }
      this.wishlistItems = wishlistItems;
    },
    async analyzeGpx() {
      this.gpxLoading = true;
      const result = await BackendService.analyzeGpx(this.gpxFile);

      if (result.proposedTitle) this.title = result.proposedTitle;
      this.elevationUp = result.elevationUp;
      this.elevationDown = result.elevationDown;
      this.distance = parseInt(result.distance * 1000, 10);
      this.polyline = result.path.map((x) => latLng(x.latitude, x.longitude));
      if (result.proposedSummit) this.summits = [result.proposedSummit];
      if (result.startTime) this.startTime = result.startTime;
      if (result.endTime) this.endTime = result.endTime;
      if (result.hikeDate) this.hikeDate = new Date(result.hikeDate).toISOString();

      this.uploadGpx = false;
      this.gpxLoading = false;
    },
    async getActivity() {
      if (!this.activityId) return;
      this.loading = true;
      this.activityToUpdate = await BackendService.getActivity(this.activityId);
      this.distance = this.activityToUpdate.distance;
      this.summits = this.activityToUpdate.summits;
      this.title = this.activityToUpdate.title;
      this.hikeDate = this.activityToUpdate.hikeDate;
      this.startTime = this.activityToUpdate.startTime;
      this.endTime = this.activityToUpdate.endTime;
      this.elevationUp = this.activityToUpdate.elevationUp;
      this.elevationDown = this.activityToUpdate.elevationDown;
      this.notes = this.activityToUpdate.notes;
      this.rating = this.activityToUpdate.rating;
      this.loading = false;
    },
  },
  computed: {
    updateMode() {
      return this.activityId !== undefined;
    },
  },
  mounted() {
    if (this.activityId) this.getActivity();
  },
};
</script>
