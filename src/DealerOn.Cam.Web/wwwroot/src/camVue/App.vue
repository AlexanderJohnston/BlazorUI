<template>
  <div id="app">
    <v-app>
      <v-toolbar dark color="primary">
        <v-toolbar-title>CAM Importer</v-toolbar-title>
      </v-toolbar>
      <v-content>
        <v-container fluid grid-list-lg>
          <v-layout>
            <status />
            <regions />
            <selectedDealer />
          </v-layout>
        </v-container>
      </v-content>
    </v-app>
  </div>
</template>

<script>
import Status from './components/Status'
import Regions from './components/Regions'
import SelectedDealer from './components/SelectedDealer'
export default {
  name: 'App',
  components: {
    Status,
    Regions,
    SelectedDealer
  },
  computed: {
    web() {
      return this.Web
    }
  }
}
</script>