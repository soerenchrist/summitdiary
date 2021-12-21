<template>
  <div style="padding: 12px;">
    <h3>Gipfelhöhen</h3>
    <div style="width: 100%;" class="chart-container">
      <canvas id="heightsChart" />
    </div>
    <div class="nodata" v-if="!loading && stats.length === 0">
      Keine Gipfel bestiegen
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
      this.stats = await BackendService.getSummitHeightStats();
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
        type: 'bar',
        data: {
          labels,
          datasets: [{
            label: 'Gipfel pro Höhe',
            data,
            backgroundColor: this.colors,
          }],
        },
        options: {
          indexAxis: 'y',
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

      const ctx = document.getElementById('heightsChart');
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
div.nodata {
  padding-top: 10px;
  padding-bottom: 10px;
}
</style>
