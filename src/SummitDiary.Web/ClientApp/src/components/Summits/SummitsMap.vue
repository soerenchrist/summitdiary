<template>
  <div style="height: 600px;">
    <l-map
      ref="map"
      :zoom="zoom"
      :center="center"
      :maxZoom="16"
      style="height: 600px"
      @ready="mapLoaded"
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
import { latLng, LatLngBounds } from 'leaflet';
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
    fitBounds() {
      if (!this.autoCenter) return;
      if (this.summits.length === 0) return;

      const coords = [];
      this.summits.forEach((summit) => {
        coords.push(latLng(summit.latitude, summit.longitude));
      });

      const bounds = new LatLngBounds(coords);
      this.$refs.map.fitBounds(bounds, { padding: [50, 50] });
    },
    mapLoaded() {
      if (this.summits) {
        this.fitBounds();
      }
    },
  },
  watch: {
    summits() {
      this.fitBounds();
    },
  },
};
</script>
