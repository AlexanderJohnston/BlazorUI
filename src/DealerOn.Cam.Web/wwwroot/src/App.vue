<template>
  <div id="app">
    <v-app>
      <v-toolbar dark color="primary">
        <v-toolbar-title>CAM Importer</v-toolbar-title>
      </v-toolbar>
      <v-content>
        <v-container fluid grid-list-lg>
          <v-layout>
            <status :query="Web.status">
              <!-- <import-when :caption="'last'" :when=""></import-when>
              <import-when :caption="'next'" :when=""></import-when> -->
            </status>
            <regions />
            <selectedDealer />
          </v-layout>
        </v-container>
      </v-content>
    </v-app>
  </div>
</template>

<script>
import Status from './components/Status.js'
import ImportWhen from './components/ImportWhen.vue'
import Regions from './components/Regions.vue'
import SelectedDealer from './components/SelectedDealer.vue'
// import Queries from "./queries.js";
// import Web from "./web.js";
export default {
  name: 'App',
  // mixins: [Web],
  components: {
    Status,
    ImportWhen,
    Regions,
    SelectedDealer
  },
  data () {
    return {
      
    }
  },
  mounted () {
    console.log(this.Web.status)
  }
}
</script>