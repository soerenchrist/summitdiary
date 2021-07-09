<template>
  <div style="margin: 10px;" class="chart-container">
    <canvas id="heightChart" />
  </div>
</template>

<script>
import {
  Chart,
} from 'chart.js';

export default {
  props: {
    path: Array,
  },
  data: () => ({
    chart: null,
  }),
  methods: {
    loadChart() {
      if (!this.path || this.path.length === 0) return;

      if (this.chart) this.chart.destroy();
      const data = [];
      this.path.forEach((x) => {
        this.data.push({
          x: x.timestamp,
          y: x.elevation,
        });
      });

      const config = {
        type: 'line',
        data: {
          datasets: [{
            label: 'Höhenprofil',
            data,
            fill: false,
            cubicInterpolationMode: 'monotone',
            borderColor: 'green',
            tension: 0.1,
          }],
        },
      };

      const ctx = document.getElementById('heightChart');
      this.chart = new Chart(ctx, config);
    },
  },
  watch: {
    path() {
      this.loadChart();
    },
  },
};
</script>
