<template>
  <div style="height: 400px;">
    <l-map
      :zoom="zoom"
      :center="center"
      style="height: 400px"
      @click="onClick"
      :options="mapOptions">
      <l-tile-layer
        :url="url"
        :attribution="attribution" />
      <l-marker v-if="position"
        :lat-lng="position" />
    </l-map>
  </div>
</template>

<script>
import { latLng } from 'leaflet';

export default {
  props: {
    position: Object,
  },
  data: () => ({
    zoom: 8,
    center: latLng(47.41322, -1.219482),
    url: 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
    attribution:
      '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
    mapOptions: {
      zoomSnap: 0.5,
    },
  }),
  watch: {
    position() {
      if (this.position) {
        this.center = this.position;
      }
    },
  },
  methods: {
    onClick(args) {
      this.$emit('positionSelected', args.latlng);
    },
  },
};
</script>
