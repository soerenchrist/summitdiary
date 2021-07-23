<template>
<div style="padding: 12px;">
  <h3>Wunschliste</h3>
  <p class="nodata" v-if="wishlist.length === 0">Aktuell keine Gipfel auf der Wunschliste</p>
  <v-simple-table v-if="wishlist.length > 0">
    <template v-slot:default>
      <thead>
        <tr>
          <th class="text-left">
            Name
          </th>
          <th class="text-left">
            Höhe
          </th>
          <th class="text-left">
            Land
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in wishlist" :key="item.id"
          @click="rowSelected(item)"
          class="pointerRow">
          <td>{{item.summit.name}}</td>
          <td>{{item.summit.height}} m</td>
          <td>{{item.summit.country.name}}</td>
        </tr>
      </tbody>
    </template>
  </v-simple-table>
</div>
</template>

<script>
import BackendService from '../../services/BackendService';

export default {
  data: () => ({
    wishlist: [],
    loading: false,
  }),
  methods: {
    async fetchWishlist() {
      this.loading = true;
      this.wishlist = await BackendService.getWishlist();
      this.loading = false;
    },
    rowSelected(item) {
      this.$router.push(`/summits/${item.summit.id}`);
    },
  },
  mounted() {
    this.fetchWishlist();
  },
};
</script>

<style scoped>
.pointerRow {
  cursor: pointer;
}
p.nodata {
  margin-top: 10px;
  margin-bottom: 10px;
}
</style>
