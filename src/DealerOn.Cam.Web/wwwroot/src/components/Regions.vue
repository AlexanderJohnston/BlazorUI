<template>
  <v-flex md3>
    <filterd-regions v-for="region in matches" :key="region.name" :region="region"></filterd-regions>
  </v-flex>
</template>

<script>
import FilterdRegions from './FilteredRegions'
export default {
  name: 'Regions',
  components: {
    FilterdRegions
  },
  data () {
    return {
      loaded: false
    }
  },
  computed: {
    matches() {
      return [
        {
          name: 'Region 1',
          dealers: [
            {
              name: 'Dealer 1',
              dealerId: '123454',
              isEnrolled: true,
              isMatch: true,
              isSelected: true
            },
            {
              name: 'Dealer 2',
              dealerId: '56874',
              isEnrolled: false,
              isMatch: false,
              isSelected: false
            }
          ]
        },
        {
          name: 'Region 2',
          dealers: [
            {
              name: 'Dealer 1',
              dealerId: '63475',
              isEnrolled: false,
              isMatch: false,
              isSelected: false
            },
            {
              name: 'Dealer 2',
              dealerId: '12589',
              isEnrolled: false,
              isMatch: false,
              isSelected: false
            }
          ]
        }
      ]
    }
  }
}
</script>