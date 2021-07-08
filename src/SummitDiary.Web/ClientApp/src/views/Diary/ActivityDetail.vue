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
    </v-sheet>
  </v-container>
</template>

<script>
import BackendService from '../../services/BackendService';

export default {
  props: {
    activityId: String,
  },
  data: () => ({
    loading: false,
    activity: null,
  }),
  methods: {
    async fetchActivity() {
      this.loading = true;
      this.activity = await BackendService.getActivity(this.activityId);
      this.loading = false;
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
  },
};
</script>
