<template>
  <v-card>
    <v-img
      height="250"
      :src="imageUrl"></v-img>
    <v-card-title :class="{ 'link-title': link }" @click="gotoSummit">
      {{summit.name}} ({{summit.height}} m)
    </v-card-title>
    <v-card-text>
      {{summit.country.name}}, {{summit.region.name}}
    </v-card-text>
  </v-card>
</template>

<script>
import BackendService from '../../services/BackendService';

export default {
  props: {
    summit: Object,
    link: Boolean,
  },
  data: () => ({
    imageUrl: 'https://keepitlocalcc.com/wp-content/uploads/2019/11/placeholder.png',
  }),
  methods: {
    async getImage() {
      try {
        const response = await BackendService.getSummitImage(this.summit.id);
        this.imageUrl = response.url;
      // eslint-disable-next-line no-empty
      } catch (e) { }
    },
    gotoSummit() {
      if (this.link) {
        this.$router.push(`/summits/${this.summit.id}`);
      }
    },
  },
  mounted() {
    this.getImage();
  },
};
</script>

<style scoped>
.link-title {
  cursor: pointer;
}
</style>
