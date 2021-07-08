<template>
  <v-container>
    <v-sheet class="mainSheet" min-height="70vh" rounded="lg">
      <v-row>
        <v-col>
          <h1>Gipfel anlegen</h1>
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-form
            ref="form"
            v-model="valid">
            <v-text-field
              v-model="name"
              :rules="nameRules"
              label="Name"
              required
              dense
              outlined />

            <v-text-field
              v-model="height"
              type="number"
              label="Höhe"
              :rules="heightRules"
              required
              dense
              outlined />

            <v-select
              v-model="country"
              :items="countries"
              :rules="[v => !!v || 'Land wird benötigt']"
              label="Land"
              required
              dense
              return-object
              item-text="name"
              outlined />
            <v-select
              v-model="region"
              :items="regions"
              :rules="[v => !!v || 'Region wird benötigt']"
              label="Region"
              required
              dense
              return-object
              item-text="name"
              outlined />
            <v-text-field
              v-model="latitude"
              type="number"
              label="Latitude"
              :rules="latitudeRules"
              required
              dense
              outlined />
            <v-text-field
              v-model="longitude"
              type="number"
              label="Longitude"
              :rules="longitudeRules"
              required
              dense
              outlined />

            <v-btn color="success"
                  :disabled="!valid"
                  @click="saveSummit">
              Speichern
            </v-btn>
          </v-form>
        </v-col>
        <v-col>
          <selection-map :position="position"
            @positionSelected="positionChanged" />
        </v-col>
      </v-row>
    </v-sheet>
  </v-container>
</template>

<script>
import { latLng } from 'leaflet';
import SelectionMap from '../../components/Common/SelectionMap.vue';
import BackendService from '../../services/BackendService';

export default {
  components: { SelectionMap },
  data: () => ({
    valid: true,
    name: '',
    height: 0,
    country: null,
    region: null,
    position: undefined,
    countries: [],
    regions: [],
    latitude: null,
    longitude: null,

    latitudeRules: [
      (v) => !!v || 'Latitude wird benötigt',
      (v) => (v && v >= -90 && v <= 90) || 'Latitude muss zwischen -90 und 90 liegen',
    ],
    longitudeRules: [
      (v) => !!v || 'Longitude wird benötigt',
      (v) => (v && v >= -180 && v <= 180) || 'Longitude muss zwischen -180 und 180 liegen',
    ],
    nameRules: [
      (v) => !!v || 'Name wird benötigt',
    ],
    heightRules: [
      (v) => !!v || 'Höhe wird benötigt',
      (v) => (v && v > 0) || 'Höhe muss positiv sein',
    ],
  }),
  watch: {
    position(pos) {
      if (pos) {
        this.latitude = pos.lat;
        this.longitude = pos.lng;
      }
    },
    latitude(lat) {
      if (this.longitude) {
        this.position = latLng(lat, this.longitude);
      }
    },
    longitude(lng) {
      if (this.latitude) {
        this.position = latLng(this.latitude, lng);
      }
    },
  },
  methods: {
    positionChanged(pos) {
      this.position = pos;
    },
    async fetchCountries() {
      this.countries = await BackendService.getCountries();
    },
    async fetchRegions() {
      this.regions = await BackendService.getRegions();
    },
    async saveSummit() {
      const summit = {
        name: this.name,
        height: this.height,
        countryId: this.country.id,
        regionId: this.region.id,
        latitude: this.latitude,
        longitude: this.longitude,
      };
      console.log(summit);

      const created = await BackendService.createSummit(summit);
      this.$router.push(`summits/${created.id}`);
    },
  },
  mounted() {
    this.fetchCountries();
    this.fetchRegions();
  },
};
</script>
