import Timeline from "totem-timeline";

export default {
  startImport: Timeline.query(class {
    importing = false;
    starting = false;
    started = false;
    startError = null;

    [":statusLoaded"](e) {
      this.importing = e.importing;
    }

    [":startImport"](e) {
      this.importing = true;
      this.starting = true;
      this.started = false;
      this.startError = null;
    }

    [":importStarted"](e) {
      this.starting = false;
      this.started = true;
    }

    [":startImportFailed"](e) {
      this.importing = false;
      this.starting = false;
      this.startError = e.error;
    }
  }),

  filter: Timeline.query(class {
    terms = "";

    [":filterChanged"](e) {
      this.terms = e.newValue;
    }
  }),

  filteredRegions: Timeline.query(class {
    _regions = [];
    _dealersById = {};
    _filter = "";
    _selectedDealerId = null;
    matches = [];

    [":regionsLoaded"](e) {
      this._regions = e.regions;

      this.applyFilter();
    }

    [":filterChanged"](e) {
      this._filter = e.newValue.toLowerCase();

      this.applyFilter();
    }

    [":dealerSelected"](e) {
      let current = this._selectedDealerId;
      let next = e.dealerId;

      this._selectedDealerId = next;

      if(current) {
        this._dealersById[current].isSelected = false;
      }

      if(next) {
        this._dealersById[next].isSelected = true;
      }
    }

    applyFilter() {
      this._dealersById = {};

      let match = value => value.toLowerCase().includes(this._filter);

      this.matches = this._regions.map(region => {
        let dealers = [];
        let dealerMatched = false;

        for(let { dealerId, name, isEnrolled } of region.dealers) {
          let isSelected = dealerId === this._selectedDealerId;

          let isMatch = match(name) || match(dealerId);

          dealerMatched = dealerMatched || isMatch;

          let dealer = { dealerId, name, isEnrolled, isSelected, isMatch };

          dealers.push(dealer);

          this._dealersById[dealerId] = dealer;
        }

        return { name: region.name, dealers, isMatch: dealerMatched };
      });
    }
  }),

  selectedDealer: Timeline.query(class {
    _detailsLinksById = null;
    dealerId = null;
    detailsLink = null;

    [":regionsLoaded"](e) {
      this._detailsLinksById = {};

      for(let region of e.regions) {
        for(let dealer of region.dealers) {
          this._detailsLinksById[dealer.dealerId] = dealer.detailsLink;
        }
      }
    }

    [":dealerSelected"](e) {
      this.dealerId = e.dealerId;
      this.detailsLink = this._detailsLinksById[e.dealerId];
    }
  }),

  changeEnrollment: Timeline.query(class {
    dealerId = null;
    home = { enrolled: false, wasEnrolled: false };
    conditional = { enrolled: false, wasEnrolled: false };
    saving = false;
    saved = false;
    saveError = null;

    [":dealerDetailsLoaded"](e) {
      if(e.dealerId !== this.dealerId) {
        this.saving = false;
        this.saved = false;
        this.saveError = null;
      }

      this.dealerId = e.dealerId;
      this.enrolledOnHome = e.enrolledOnHome;
      this.enrolledOnConditional = e.enrolledOnConditional;
    }

    [":changeEnrollment"](e) {
      this.saving = true;
      this.saved = false;
      this.saveError = null;

      this.home.wasEnrolled = this.home.enrolled;
      this.home.enrolled = e.home;

      this.conditional.wasEnrolled = this.conditional.enrolled;
      this.conditional.enrolled = e.conditional;
    }

    [":enrollmentChanged"](e) {
      this.saving = false;
      this.saved = true;

      this.home.wasEnrolled = false;
      this.conditional.wasEnrolled = false;
    }

    [":changeEnrollmentFailed"](e) {
      this.saving = false;
      this.saved = false;
      this.saveError = e.error;

      this.home.enrolled = this.home.wasEnrolled;
      this.home.wasEnrolled = false;

      this.conditional.enrolled = this.conditional.wasEnrolled;
      this.conditional.wasEnrolled = false;
    }
  }),

  navigation: Timeline.query(class {
    importSelected = true;
    assetsSelected = false;

    [":navigatedToImport"]() {
      this.importSelected = true;
      this.assetsSelected = false;
    }

    [":navigatedToAssets"]() {
      this.importSelected = false;
      this.assetsSelected = true;
    }
  })
};