<template>
  <v-list dense subheader>
    <v-subheader>{{ region.name }}</v-subheader>
    <v-list-tile
    v-for="(dealer, i) in region.dealers"
    :key="`${region.name}-dealer-${i}`"
    @click="">
      <v-list-tile-title
      :class="className(dealer.isEnrolled, dealer.isSelected, dealer.isMatch)">
        {{ dealer.dealerId }} {{ dealer.name }}
      </v-list-tile-title>
    </v-list-tile>
  </v-list>
</template>

<script>
export default {
  name: 'FilteredRegions',
  props: {
    region: Object
  },
  data () {
    return {

    }
  },
  methods: {
    className(isEnrolled, isSelected, isMatch) {
      return this.classy("dealer", { isEnrolled, isSelected, isMatch })
    }
  }
}
</script>
