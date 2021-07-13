<template>
<div style="padding: 12px;">
  <v-progress-linear indeterminate v-if="loading" />
  <h3>Länder</h3>
  <div style="width: 100%;" class="chart-container">
    <canvas id="countryChart" />
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
    valueType: 'elevation',
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
  }),
  watch: {
    stats() {
      this.loadChart();
    },
  },
  methods: {
    async fetchStats() {
      this.loading = true;
      this.stats = await BackendService.getCountryStats({
        valueType: this.valueType,
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
    this.fetchStats();
  },
};
</script>
