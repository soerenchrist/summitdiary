<template>
  <l-map
    :zoom="zoom"
    :center="center"
    :options="mapOptions">
    <l-tile-layer
      :url="url"
      :attribution="attribution" />
    <l-marker v-for="summit in summits" :key="summit.id"
            :lat-lng="toLatLong(summit)" />
  </l-map>
</template>

<script>
import { latLng } from 'leaflet';

export default {
  props: {
    summits: Array,
    loading: Boolean,
  },
  data: () => ({
    zoom: 13,
    center: latLng(47.41322, -1.219482),
    url: 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
    attribution:
      '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
    mapOptions: {
      zoomSnap: 0.5,
    },
  }),
  methods: {
    toLatLong(summit) {
      return latLng(summit.latitude, summit.longitude);
    },
  },
};
</script>
