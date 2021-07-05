<template>
  <v-app-bar
    app
    color="blue"
    flat
  >
    <v-container class="py-0 fill-height">
      <v-btn icon @click="toggleDrawer">
        <v-icon>
          mdi-menu
        </v-icon>
      </v-btn>
      <v-spacer />
    </v-container>
  </v-app-bar>
</template>

<script>
export default {
  computed: {
    drawer() {
      return this.$store.getters.getDrawer;
    },
  },
  methods: {
    toggleDrawer() {
      this.$store.commit('toggleDrawer', !this.drawer);
    },
  },
};
</script>
