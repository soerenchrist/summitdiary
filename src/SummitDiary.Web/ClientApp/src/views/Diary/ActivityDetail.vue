<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <v-row>
        <v-progress-linear indeterminate v-if="loading" />
        <v-col v-if="activity !== null">
          <h1>{{activity.title}}</h1>
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
        <v-col :sm="6" :md="3" v-for="summit in activity.summits" :key="summit.id">
          <summit-card :summit="summit" />
        </v-col>
        <v-col>
          <summits-map :summits="activity.summits" :autoCenter="true" :polyline="polyline" />
        </v-col>
      </v-row>
    </v-sheet>
  </v-container>
</template>

<script>
import { latLng } from 'leaflet';
import SummitCard from '../../components/Summits/SummitCard.vue';
import SummitsMap from '../../components/Summits/SummitsMap.vue';
import BackendService from '../../services/BackendService';

export default {
  components: {
    SummitCard,
    SummitsMap,
  },
  props: {
    activityId: String,
  },
  data: () => ({
    loading: false,
    activity: null,
    polyline: [],
  }),
  methods: {
    async fetchActivity() {
      this.loading = true;
      this.activity = await BackendService.getActivity(this.activityId);
      this.loading = false;
    },
    async fetchPath() {
      const response = await BackendService.getActivityPath(this.activityId);
      this.polyline = response.path.map((x) => latLng(x.latitude, x.longitude));
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
