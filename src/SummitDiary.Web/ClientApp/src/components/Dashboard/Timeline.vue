<template>
<div class="outer">
  <v-row>
    <v-col>
      <v-progress-linear indeterminate v-if="loading" />
    </v-col>
  </v-row>
  <v-row style="padding: 12px">
    <v-col :cols="4">
      <h3>Timeline</h3>
    </v-col>
    <v-col>
      <v-select
        :items="timeTypes"
        v-model="timeType"
        item-text="name"
        return-object
        label="Zeit"
        class="float-right"
        dense outlined />
      <v-select
        :items="valueTypes"
        v-model="valueType"
        item-text="name"
        return-object
        label="Einheit"
        class="mr-2 float-right"
        dense outlined />
    </v-col>
  </v-row>
  <v-row>
    <v-col>
      <div style="width: 98%;" class="chart-container">
        <canvas id="timelineChart" />
      </div>
    </v-col>
  </v-row>
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
    timeTypes: [
      { key: 'year', name: 'Jahr' },
      { key: 'month', name: 'Monat' },
      { key: 'all', name: 'Alles' },
    ],
    valueTypes: [
      { key: 'elevation', name: 'Höhenmeter' },
      { key: 'distance', name: 'Distanz' },
    ],
    timeType: null,
    valueType: null,
    chart: null,
    loading: true,
  }),
  watch: {
    timeline() {
      this.loadChart();
    },
    timeType() {
      this.fetchTimeline();
    },
    valueType() {
      this.fetchTimeline();
    },
  },
  methods: {
    async fetchTimeline() {
      this.loading = true;
      this.timeline = await BackendService.getTimeline({
        timeType: this.timeType.key,
        valueType: this.valueType.key,
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

      const timeType = this.timeType.key;

      const timeUnit = timeType === 'year' || timeType.key === 'all'
        ? 'month' : 'week';

      const displayFormat = timeType.key === 'year' || timeType.key === 'all'
        ? 'MM YYYY' : 'DD.MM.YYYY';

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
                unit: timeUnit,
                displayFormats: {
                  minute: displayFormat,
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
    [this.timeType] = this.timeTypes;
    [this.valueType] = this.valueTypes;
    this.fetchTimeline();
  },
};
</script>

<style scoped>
.row {
  padding: 0;
}
.col {
  padding: 0;
}
.outer {
  padding: 12px;
}
.v-text-field {
  width: 150px;
}
</style>
