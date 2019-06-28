<template>
  <v-flex>
    <v-list>
      <v-list-tile>
        <v-list-tile-sub-title>Dealer ID</v-list-tile-sub-title>
        <v-list-tile-title>{{ data.dealerId }}</v-list-tile-title>
      </v-list-tile>
      <v-list-tile>
        <v-list-tile-sub-title>Code</v-list-tile-sub-title>
        <v-list-tile-title>{{ data.code }}</v-list-tile-title>
      </v-list-tile>
      <v-list-tile>
        <v-list-tile-sub-title>Region</v-list-tile-sub-title>
        <v-list-tile-title>{{ data.region }}</v-list-tile-title>
      </v-list-tile>
      <v-list-tile href="data.manifestLink" target="_blank">
        <v-list-tile-sub-title>Manifest</v-list-tile-sub-title>
        <v-list-tile-title>{{ data.manifestLink }}</v-list-tile-title>
      </v-list-tile>
    </v-list>


    <!-- <section id="dealer-details-values">
      <div class="dealer-details-value">
        <span class="dealer-details-value-caption">Dealer ID:</span>
        <span class="dealer-details-value-data">{{data.dealerId}}</span>
      </div>
  
      <div class="dealer-details-value">
        <span class="dealer-details-value-caption">Code:</span>
        <span class="dealer-details-value-data">{{data.code}}</span>
      </div>
  
      <div class="dealer-details-value">
        <span class="dealer-details-value-caption">Region:</span>
        <span class="dealer-details-value-data">{{data.region}}</span>
      </div>
  
      <div class="dealer-details-value">
        <span class="dealer-details-value-caption">Manifest:</span>
        <a class="dealer-details-value-data" :href="data.manifestLink" target="_blank">{{data.manifestLink}}</a>
      </div>
    </section> -->
    
    <section id="dealer-details-enrollment">
      <h3>Enrollment</h3>
      <div id="dealer-details-enrollment-message" :class="classy({ saving, saved, saveError })">
        {{saved ? "saved!" : (saveError ? "error" : null)}}
      </div>
      <div id="dealer-details-enrollment-body">
        <div class="dealer-details-enrollment-type">
          <v-checkbox id="dealer-details-enrollment-home" label="Home page" :value="data.enrolledOnHome"></v-checkbox>
        </div>
  
        <div class="dealer-details-enrollment-type">
          <v-checkbox id="dealer-details-enrollment-conditional" label="SRP/VDP" :value="data.enrolledOnConditional"></v-checkbox>
        </div>
      </div>
    </section>

    <section v-if="data.manifestDownloadError || data.campaignImportError" id="dealer-details-errors">
      <h3>Errors</h3>
        <section v-if="data.manifestDownloadError" id="dealer-details-manifest-error">
          <h4>Failed to download the manifest</h4>
          <span id="dealer-details-manifest-error-message">
            {{data.manifestDownloadError}}
          </span>
        </section>
        
        <section v-if="data.campaignImportError" id="dealer-details-campaign-error">
          <h4>Failed to import campaigns</h4>
          <span id="dealer-details-campaign-error-message">
            {{data.campaignImportError}}
          </span>
        </section>
    </section>
  </v-flex>
</template>

<script>
export default {
  name: 'SelectedDealers',
  data () {
    return {
      data: {}
    }
  }
}
</script>