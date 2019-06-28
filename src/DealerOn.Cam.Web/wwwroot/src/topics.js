import Http from "../submodules/totem/http";
import Timeline from "../submodules/totem/timeline";

Timeline.topic(class {
  async [":startImport"](e, then) {
    try {
      await Http.post("/api/imports");

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
      await Http.put(`/api/enrollment/${pages}/${dealerId}`);

      then("enrollmentChanged", { dealerId, home, conditional });
    }
    catch(error) {
      then("changeEnrollmentFailed", { dealerId, home, conditional, error });
    }
  }
});