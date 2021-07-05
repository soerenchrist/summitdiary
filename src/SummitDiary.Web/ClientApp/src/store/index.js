import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    drawer: null,
  },
  mutations: {
    toggleDrawer(state, drawer) {
      state.drawer = drawer;
    },
  },
  getters: {
    getDrawer(state) {
      return state.drawer;
    },
  },
  actions: {
  },
  modules: {
  },
});
