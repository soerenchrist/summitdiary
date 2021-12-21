<template>
<div style="padding: 12px;">
  <v-row>
    <v-col>
      <v-progress-linear indeterminate v-if="loading" />
    </v-col>
  </v-row>
  <v-row style="padding: 12px;">
    <v-col :cols="4">
      <h3>Länder</h3>
    </v-col>
    <v-col>
      <v-select
        :items="valueTypes"
        v-model="valueType"
        item-text="name"
        return-object
        label="Einheit"
        class="float-right"
        dense outlined />
      </v-col>
  </v-row>
  <div style="width: 100%;" class="chart-container">
    <canvas id="countryChart" />
  </div>
  <div class="nodata" v-if="!loading && stats.length === 0">
    Keine Aktivitäten
  </div>
</div>
</template>

<script>
import {
  Chart,
} from 'chart.js';
import BackendService from '../../services/BackendService';

export default {
  data: () => ({
    stats: [],
    valueType: null,
    chart: null,
    loading: true,
    colors: [
      '#2196f3',
      '#3f51b5',
      '#673ab7',
      '#9c27b0',
      '#e91e63',
      '#f44336',
    ],
    valueTypes: [
      { key: 'elevation', name: 'Höhenmeter' },
      { key: 'distance', name: 'Distanz' },
    ],
  }),
  watch: {
    stats() {
      this.loadChart();
    },
    valueType() {
      this.fetchStats();
    },
  },
  methods: {
    async fetchStats() {
      this.loading = true;
      this.stats = await BackendService.getCountryStats({
        valueType: this.valueType.key,
      });
      this.loading = false;
    },
    loadChart() {
      if (!this.stats || this.stats.length === 0) return;
      if (this.chart) this.chart.destroy();

      const data = [];
      const labels = [];

      this.stats.forEach((x) => {
        data.push(x.value);
        labels.push(x.name);
      });

      const config = {
        type: 'pie',
        data: {
          labels,
          datasets: [{
            label: 'Höhenmeter nach Land',
            data,
            backgroundColor: this.colors,
          }],
        },
        options: {
          plugins: {
            title: {
              display: false,
            },
            legend: {
              display: false,
            },
          },
          responsive: true,
        },
      };

      const ctx = document.getElementById('countryChart');
      this.chart = new Chart(ctx, config);
    },
  },
  mounted() {
    [this.valueType] = this.valueTypes;
    this.fetchStats();
  },
};
</script>

<style scoped>
.v-text-field {
  width: 150px;
}
.row {
  padding: 0;
}
.col {
  padding: 0;
}
div.nodata {
  padding-top: 10px;
  padding-bottom: 10px;
}
</style>
