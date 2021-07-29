<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <v-row>
        <v-col>
          <h1>Tour planen</h1>
        </v-col>
      </v-row>
      <v-row>
        <v-col :cols="8">
          <v-toolbar
            dense>
            <v-btn icon :disabled="path.length === 0" @click="undo">
              <v-icon>mdi-undo</v-icon>
            </v-btn>
          </v-toolbar>
          <l-map
            :zoom="zoom"
            :center="center"
            style="height: 400px"
            :maxZoom="16"
            :options="mapOptions"
            @click="onClick">
            <l-tile-layer
              :url="url"
              :attribution="attribution" />
            <l-polyline v-if="path && path.length > 0"
              :lat-lngs="path"
              color="#2196f3" />
            <l-marker v-if="start" :lat-lng="start" />
            <l-marker v-if="end" :lat-lng="end" />
          </l-map>
        </v-col>
        <v-col>
          <v-progress-linear indeterminate v-if="loading" />
          <v-list>
            <v-list-item>
              <v-list-item-content>
                <v-list-item-title>
                  {{distance.toFixed(2)}} km
                </v-list-item-title>
                <v-list-item-subtitle>
                  Distanz
                </v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
            <v-list-item>
              <v-list-item-content>
                <v-list-item-title>
                  {{elevationUp.toFixed(2)}} m
                </v-list-item-title>
                <v-list-item-subtitle>
                  Höhenmeter auf
                </v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
            <v-list-item>
              <v-list-item-content>
                <v-list-item-title>
                  {{elevationDown.toFixed(2)}} km
                </v-list-item-title>
                <v-list-item-subtitle>
                  Höhenmeter ab
                </v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </v-col>
      </v-row>

    </v-sheet>
  </v-container>
</template>

<script>
import { latLng } from 'leaflet';
import BackendService from '../services/BackendService';

export default {
  data: () => ({
    zoom: 8,
    path: [],
    center: latLng(47.2285, 11.9135),
    url: 'https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png',
    attribution:
      '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors',
    mapOptions: {
      zoomSnap: 0.5,
    },
    loading: false,
    distance: 0,
    elevationUp: 0,
    elevationDown: 0,
  }),
  watch: {
    path() {
      if (this.path.length > 1) this.analyze();
    },
  },
  computed: {
    start() {
      if (this.path.length === 0) return null;
      return this.path[0];
    },
    end() {
      if (this.path.length <= 1) return null;
      return this.path[this.path.length - 1];
    },
  },
  methods: {
    onClick(args) {
      this.path.push(args.latlng);
    },
    undo() {
      if (this.path.length === 0) return;

      this.path.pop();
    },
    async analyze() {
      this.loading = true;
      const path = this.path.map((x) => ({
        latitude: x.lat,
        longitude: x.lng,
      }));
      const response = await BackendService.analyzePath({ points: path });
      this.distance = response.distance;
      this.elevationUp = response.elevationUp;
      this.elevationDown = response.elevationDown;
      this.loading = false;
    },
  },
};
</script>
