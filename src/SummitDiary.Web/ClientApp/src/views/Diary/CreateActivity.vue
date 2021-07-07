<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <v-row>
        <v-col>
          <h1>Aktivität anlegen</h1>
        </v-col>
        <v-col>
          <v-btn class="pageButton"
            @click="uploadGpx = true">GPX hochladen</v-btn>
        </v-col>
        <v-progress-linear indeterminate v-if="loading" />
      </v-row>
      <v-row>
        <v-col>
          <v-form v-model="valid">
            <v-text-field
              v-model="title"
              :rules="[v => !!v || 'Titel wird benötigt']"
              label="Titel"
              required
              dense
              outlined />

            <summits-autocomplete
              @summitsChanged="summitsChanged"
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
              @click="saveActivity"
              :disabled="!valid || summits.length === 0">
              Speichern
            </v-btn>
          </v-form>
        </v-col>
        <v-col>
          <summits-map :summits="summits"
            :autoCenter="true"
            :loading="false" />
        </v-col>
      </v-row>
    </v-sheet>
    <v-dialog
      width="500"
      v-model="uploadGpx">
      <v-card>
        <v-card-title>GPX hochladen</v-card-title>
        <v-card-text>
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
          <v-btn text @click="uploadGpxFiles"
            :disabled="gpxFile === null">Hochladen</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
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
    summitsChanged(summits) {
      this.summits = summits;
    },
    async saveActivity() {
      const activity = {
        title: this.title,
        hikeDate: this.hikeDate,
        elevationUp: this.elevationUp,
        elevationDown: this.elevationDown,
        distance: this.distance,
        startTime: this.startTime,
        endTime: this.endTime,
        rating: 1,
        summitIds: this.summits.map((s) => s.id),
      };
      this.loading = true;
      await BackendService.createActivity(activity);
      this.loading = false;

      this.$router.go(-1);
    },
    async uploadGpxFiles() {
      const result = await BackendService.analyzeGpx(this.gpxFile);

      if (result.proposedTitle) this.title = result.proposedTitle;
      this.elevationUp = result.elevationUp;
      this.elevationDown = result.elevationDown;
      this.distance = parseInt(result.distance * 1000, 10);
      this.uploadGpx = false;
    },
  },
};
</script>
