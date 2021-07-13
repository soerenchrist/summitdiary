<template>
  <div style="margin: 10px; width: 90%;"  class="chart-container">
    <canvas id="heightChart" />
  </div>
</template>

<script>
import moment from 'moment';
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
      const labels = [];
      this.path.forEach((x) => {
        data.push(x.elevation);
        labels.push(moment(x.dateTime));
      });

      const config = {
        type: 'line',
        data: {
          labels,
          datasets: [{
            label: 'Höhenprofil',
            data,
            fill: false,
            cubicInterpolationMode: 'monotone',
            borderColor: '#2196F3',
            tension: 0.1,
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
          scales: {
            x: {
              type: 'time',
              time: {
                parser: 'YYYY-MM-DDTHH:mm:ss',
                unit: 'minute',
                displayFormats: {
                  minute: 'HH:mm',
                },
              },
            },
          },
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
  mounted() {
    this.loadChart();
  },
};
</script>
