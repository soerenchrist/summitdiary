<template>
  <div style="height: 600px;">
    <l-map
      ref="map"
      :zoom="zoom"
      :center="center"
      :maxZoom="14"
      style="height: 600px"
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
        color="blue" />
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
  },
  watch: {
    summits(val) {
      if (!this.autoCenter) return;
      if (val.length === 0) return;

      const coords = [];
      val.forEach((summit) => {
        coords.push(latLng(summit.latitude, summit.longitude));
      });

      const bounds = new LatLngBounds(coords);
      this.$refs.map.fitBounds(bounds, { padding: [50, 50] });
    },
  },
};
</script>
