<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <v-row>
        <v-col>
          <h1>Aktivität anlegen</h1>
        </v-col>
        <v-col>
          <v-btn class="pageButton"
            v-if="step === 1"
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
                    :disabled="!valid || summits.length === 0">
                    Weiter
                  </v-btn>
                </v-form>
              </v-col>
              <v-col>
                <summits-map :summits="summits"
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
    step: 2,
    notes: '',
    rating: 0,

    elevationRules: [
      (v) => !!v || 'Höhenmeter werden benötigt',
      (v) => (v && v > 0) || 'Höhenmeter müssen positiv sein',
    ],
    distanceRules: [
      (v) => !!v || 'Distanz wird benötigt',
      (v) => (v && v > 0) || 'Distanz müssen positiv sein',
    ],
  }),
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
      const createdActivity = await BackendService.createActivity(activity);
      this.uploadGpxFile(createdActivity.id);

      this.loading = false;

      this.$router.go(-1);
    },
    async uploadGpxFile(activityId) {
      if (this.gpxFile) {
        await BackendService.uploadGpx(activityId, this.gpxFile);
      }
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
  },
};
</script>
