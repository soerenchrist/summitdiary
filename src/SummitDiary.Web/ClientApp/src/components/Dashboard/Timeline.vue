<template>
<div style="padding: 12px;">
  <v-progress-linear indeterminate v-if="loading" />
  <h3>Timeline</h3>
  <div style="width: 98%;" class="chart-container">
    <canvas id="timelineChart" />
  </div>
</div>
</template>

<script>
import moment from 'moment';
import {
  Chart,
} from 'chart.js';
import BackendService from '../../services/BackendService';

export default {
  data: () => ({
    timeline: [],
    timeType: 'year',
    valueType: 'elevation',
    chart: null,
    loading: true,
  }),
  watch: {
    timeline() {
      this.loadChart();
    },
  },
  methods: {
    async fetchTimeline() {
      this.loading = true;
      this.timeline = await BackendService.getTimeline({
        timeType: this.timeType,
        valueType: this.valueType,
      });
      this.loading = false;
    },
    loadChart() {
      if (!this.timeline || this.timeline.length === 0) return;
      if (this.chart) this.chart.destroy();

      const data = [];
      const labels = [];

      this.timeline.forEach((x) => {
        data.push(x.value);
        labels.push(moment(x.date));
      });

      const config = {
        type: 'line',
        data: {
          labels,
          datasets: [{
            label: 'Timeline',
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
                unit: 'month',
                displayFormats: {
                  minute: 'MM YYYY',
                },
              },
            },
          },
        },
      };

      const ctx = document.getElementById('timelineChart');
      this.chart = new Chart(ctx, config);
    },
  },
  mounted() {
    this.fetchTimeline();
  },
};
</script>
