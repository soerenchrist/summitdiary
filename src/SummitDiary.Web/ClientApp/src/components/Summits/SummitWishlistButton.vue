<template>
  <div>
    <v-tooltip bottom v-if="showButton">
      <template v-slot:activator="{ on, attrs }">
        <v-btn
          icon
          v-bind="attrs"
          @click="toggleWishlist"
          v-on="on">
          <v-icon
          :color="color">
            {{icon}}
          </v-icon>
        </v-btn>
      </template>
      <span>{{tooltip}}</span>
    </v-tooltip>
    <v-progress-circular indeterminate v-if="loading" />
  </div>
</template>

<script>
import BackendService from '../../services/BackendService';

export default {
  props: {
    summit: Object,
  },
  data: () => ({
    wishlistItem: null,
    loading: false,
  }),
  methods: {
    async fetchWishlistState() {
      this.loading = true;
      try {
        this.wishlistItem = await BackendService.getWishlistItemForSummit(this.summit.id);
      } finally {
        this.loading = false;
      }
    },
    async toggleWishlist() {
      this.loading = true;

      try {
        if (this.isOnWishlist) {
          await BackendService.removeFromWishlist(this.wishlistItem.id);
          this.wishlistItem = null;
        } else {
          this.wishlistItem = await BackendService.addSummitToWishlist(this.summit.id);
        }
      } finally {
        this.loading = false;
      }
    },
  },
  watch: {
    summit() {
      if (this.summit) {
        this.fetchWishlistState();
      }
    },
  },
  computed: {
    showButton() {
      return this.summit !== null && !this.loading;
    },
    isOnWishlist() {
      if (this.wishlistItem === null) return false;

      return !this.wishlistItem.finished;
    },
    icon() {
      return this.isOnWishlist ? 'mdi-heart' : 'mdi-heart-outline';
    },
    tooltip() {
      return this.isOnWishlist ? 'Von der Wunschliste entfernen'
        : 'Auf die Wunschliste setzen';
    },
    color() {
      return this.isOnWishlist ? 'red' : 'gray';
    },
  },
};
</script>
