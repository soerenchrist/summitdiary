<template>
  <div style="height: 600px;">
    <l-map
      :zoom="zoom"
      :center="center"
      style="height: 600px"
      :options="mapOptions">
      <l-tile-layer
        :url="url"
        :attribution="attribution" />
      <l-marker v-for="summit in summits" :key="summit.id"
              :lat-lng="toLatLong(summit)">
        <l-popup>
          <div>
          {{summit.name}}
          </div>
        </l-popup>
      </l-marker>
    </l-map>
  </div>
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
