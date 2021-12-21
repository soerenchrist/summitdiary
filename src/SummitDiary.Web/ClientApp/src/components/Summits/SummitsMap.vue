<template>
  <div style="height: 600px;">
    <l-map
      ref="map"
      :zoom="zoom"
      :center="center"
      :maxZoom="16"
      style="height: 600px"
      @update:bounds="boundsUpdated"
      :options="mapOptions">
      <l-tile-layer
        :url="url"
        :attribution="attribution" />
      <l-marker v-for="summit in summits" :key="summit.id"
              :lat-lng="toLatLong(summit)">
        <summit-map-popup :summit="summit" />
      </l-marker>
      <l-polyline v-if="polyline && polyline.length > 0"
        :lat-lngs="polyline"
        color="#2196F3" />
    </l-map>
  </div>
</template>

<script>
import { latLng } from 'leaflet';
import SummitMapPopup from './SummitMapPopup.vue';

export default {
  components: { SummitMapPopup },
  props: {
    summits: Array,
    loading: Boolean,
    autoCenter: Boolean,
    polyline: Array,
    center: {
      type: Object,
      required: false,
      default: () => latLng(47.2285, 11.9135),
    },
    zoom: {
      type: Number,
      required: false,
      default: 6,
    },
  },
  data: () => ({
    url: 'https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png',
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
    boundsUpdated(bounds) {
      this.$emit('boundsChanged', {
        swLat: bounds._southWest.lat,
        swLon: bounds._southWest.lng,
        neLat: bounds._northEast.lat,
        neLon: bounds._northEast.lng,
      });
    },
    fitBounds(bounds, padding) {
      this.$refs.map.fitBounds(bounds, { padding });
    },
  },
};
</script>
