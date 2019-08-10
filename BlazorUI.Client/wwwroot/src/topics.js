import Timeline from "totem-timeline";

Timeline.topic(class {
  async [":startImport"](e, then) {
    try {

      await fetch("/api/imports", { method: "POST" });

      then("importStarted");
    }
    catch(error) {
      then("startImportFailed", { error });
    }
  }
});

Timeline.topic(class {
  async [":changeEnrollment"]({ dealerId, home, conditional }, then) {
    let pages;

    if(home && conditional) {
      pages = "all";
    }
    else if(home) {
      pages = "home";
    }
    else if(conditional) {
      pages = "conditional";
    }
    else {
      pages = "none";
    }

    try {
      await fetch(`/api/enrollment/${pages}/${dealerId}`, { method: "PUT" });

      then("enrollmentChanged", { dealerId, home, conditional });
    }
    catch(error) {
      then("changeEnrollmentFailed", { dealerId, home, conditional, error });
    }
  }
});

Timeline.topic(class {
  importSelected = true;

  [":navigateToImport"](e, then) {
    if(!this.importSelected) {
      this.importSelected = true;

      then("navigatedToImport");
    }
  }

  [":navigateToAssets"](e, then) {
    if(this.importSelected) {
      this.importSelected = false;

      then("navigatedToAssets");
    }
  }
});