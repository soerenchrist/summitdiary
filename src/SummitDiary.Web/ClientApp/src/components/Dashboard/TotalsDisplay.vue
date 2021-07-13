<template>
  <v-row>
    <v-col>
      <stats-card title="Aktivitäten"
        :value="activityCount"
        color="blue" />
    </v-col>
    <v-col>
      <stats-card title="Gipfel"
        :value="summitCount"
        color="indigo" />
    </v-col>
    <v-col>
      <stats-card title="Distanz"
        :value="distance"
        color="purple" />
    </v-col>
    <v-col>
      <stats-card title="Höhenmeter"
        :value="elevation"
        color="pink" />
    </v-col>
    <v-col>
      <stats-card title="Dauer"
        :value="duration"
        color="teal" />
    </v-col>
  </v-row>
</template>

<script>
import BackendService from '../../services/BackendService';
import StatsCard from '../Common/StatsCard.vue';

export default {
  components: { StatsCard },
  data: () => ({
    totals: null,
    loading: false,
  }),
  computed: {
    activityCount() {
      return this.totals === null ? 0 : this.totals.activityCount;
    },
    summitCount() {
      return this.totals === null ? 0 : this.totals.summitCount;
    },
    distance() {
      return this.totals === null ? 0 : `${(this.totals.distance / 1000).toFixed(0)} km`;
    },
    elevation() {
      if (this.totals === null) return 0;
      if (this.totals.elevation > 10000) {
        return `${(this.totals.elevation / 1000).toFixed(0)} km`;
      }
      return `${this.totals.elevation} m`;
    },
    duration() {
      if (this.totals === null) return 0;
      const hours = Math.floor(this.totals.duration / 3600);

      return `${hours} h`;
    },
  },
  methods: {
    async fetchTotals() {
      this.loading = true;
      this.totals = await BackendService.getTotals();
      this.loading = false;
    },
  },
  mounted() {
    this.fetchTotals();
  },
};
</script>
