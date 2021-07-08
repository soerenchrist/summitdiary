<template>
  <v-navigation-drawer
    v-model="drawer"
    app
  >
    <v-sheet color="grey lighten-4"
      class="pa-4">
      <v-row>
        <v-col cols="3">
          <v-avatar
            class="mr-5"
            size="64"
          >
          </v-avatar>
        </v-col>
        <v-col class="appname-container">
          <h1 style="margin: 0">Gipfelstürmer</h1>
        </v-col>
      </v-row>
    </v-sheet>
    <v-divider />
    <v-list>
      <v-list-item
          v-for="link in links"
          :key="link.icon"
          link
          :to="link.link"
        >
        <v-list-item-icon>
          <v-icon>{{ link.icon }}</v-icon>
        </v-list-item-icon>

        <v-list-item-content>
          <v-list-item-title>{{ link.name }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>
  </v-navigation-drawer>
</template>

<script>
export default {
  data: () => ({
    drawer: null,
    links: [
      {
        icon: 'mdi-home',
        name: 'Dashboard',
        link: '/',
      },
      {
        icon: 'mdi-book',
        name: 'Tagebuch',
        link: '/activities',
      },
      {
        icon: 'mdi-summit',
        name: 'Gipfel',
        link: '/summits',
      },
      {
        icon: 'mdi-cogs',
        name: 'Settings',
        link: '/settings',
      },
    ],
  }),
  computed: {
    globalDrawer() {
      return this.$store.getters.getDrawer;
    },
  },
  watch: {
    globalDrawer() {
      this.drawer = this.globalDrawer;
    },
    drawer() {
      this.$store.commit('toggleDrawer', this.drawer);
    },
  },
};
</script>

<style lang="less" scoped>
.appname-container {
  display:flex;
  align-items: center;
  justify-content: center;
}
</style>
