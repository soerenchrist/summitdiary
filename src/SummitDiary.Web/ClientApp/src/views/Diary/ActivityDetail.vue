<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <v-row>
        <v-progress-linear indeterminate v-if="loading" />
        <v-col v-if="activity !== null">
          <h1>{{activity.title}}</h1>
        </v-col>
        <v-col>
          <v-btn icon class="pageButton" @click="confirmDeletion = true">
            <v-icon>mdi-delete</v-icon>
          </v-btn>
        </v-col>
      </v-row>
      <v-row v-if="activity !== null">
        <v-col :sm="4" :md="2">
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>
                {{new Date(activity.hikeDate).toLocaleDateString()}}
              </v-list-item-title>
              <v-list-item-subtitle>Datum</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-col>
        <v-col :sm="4" :md="2">
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>{{(activity.distance / 1000).toFixed(2)}} km</v-list-item-title>
              <v-list-item-subtitle>Distanz</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-col>
        <v-col :sm="4" :md="2">
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>{{activity.elevationUp}} m</v-list-item-title>
              <v-list-item-subtitle>Höhenmeter auf</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-col>
        <v-col :sm="4" :md="2">
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>{{activity.elevationDown}} m</v-list-item-title>
              <v-list-item-subtitle>Höhenmeter ab</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-col>
        <v-col :sm="4" :md="2">
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title>{{formatTime(activity.duration)}}</v-list-item-title>
              <v-list-item-subtitle>Dauer</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-col>
      </v-row>
      <v-row v-if="activity">
        <v-col :sm="6" :md="4" :lg="3" v-for="summit in activity.summits" :key="summit.id">
          <summit-card :summit="summit" />
        </v-col>
        <v-col>
          <summits-map :summits="activity.summits" :autoCenter="true" :polyline="polyline" />
        </v-col>
      </v-row>
      <v-row>
        <height-profile :path="path" />
      </v-row>
    </v-sheet>
    <confirmation-dialog title="Aktivität löschen?"
      content="Soll die Aktivität wirklich gelöscht werden?"
      :open="confirmDeletion"
      @confirmed="deleteActivity" />
  </v-container>
</template>

<script>
import { latLng } from 'leaflet';
import SummitCard from '../../components/Summits/SummitCard.vue';
import SummitsMap from '../../components/Summits/SummitsMap.vue';
import BackendService from '../../services/BackendService';
import ConfirmationDialog from '../../components/Common/ConfirmationDialog.vue';
import HeightProfile from '../../components/Diary/HeightProfile.vue';

export default {
  components: {
    SummitCard,
    SummitsMap,
    ConfirmationDialog,
    HeightProfile,
  },
  props: {
    activityId: String,
  },
  data: () => ({
    confirmDeletion: false,
    loading: false,
    activity: null,
    polyline: [],
    path: [],
  }),
  methods: {
    async fetchActivity() {
      this.loading = true;
      this.activity = await BackendService.getActivity(this.activityId);
      this.loading = false;
    },
    async fetchPath() {
      const response = await BackendService.getActivityPath(this.activityId);
      this.path = response.path;
      this.polyline = response.path.map((x) => latLng(x.latitude, x.longitude));
    },
    async deleteActivity() {
      await BackendService.deleteActivity(this.activityId);
      this.$router.go(-1);
    },
    formatTime(seconds) {
      const hours = Math.floor(seconds / 3600);
      seconds %= 3600;
      const minutes = Math.floor(seconds / 60);
      return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
    },
  },
  mounted() {
    this.fetchActivity();
    this.fetchPath();
  },
};
</script>
