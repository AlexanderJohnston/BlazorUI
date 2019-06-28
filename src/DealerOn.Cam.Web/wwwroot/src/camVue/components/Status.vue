<template>
  <v-flex md3 id="status">
    <v-card>
      <v-card-text>
        <v-btn
        id="start-import"
        depressed
        color="primary"
        :class="classy({ importing, starting, started, startError })"
        :disabled="importing"
        @click="">
          {{ importing || starting ? "Importing..." : "Start import" }}
        </v-btn>
        <import-when :caption="'Last'" />
        <import-when :caption="'Next'" />
        <v-text-field
        id="filter"
        type="text"
        placeholder="filter dealers..."
        :value="terms"
        @change=""/>
      </v-card-text>
    </v-card>
  </v-flex>
</template>

<script>
import ImportWhen from './ImportWhen'
export default {
  name: 'Status',
  components: {
    ImportWhen
  },
  data () {
    return {
      importing: false,
      starting: false,
      terms: null
    }
  }
}
</script>