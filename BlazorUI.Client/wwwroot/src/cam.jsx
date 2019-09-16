import React from "react";
import Moment from "moment";
import UI from "totem-timeline-react";

import "./css/cam.css";
import "./topics";
import Queries from "./queries";
import Web from "./web";

export default UI(Queries.navigation, ({ importSelected }) =>
  <div id="cam">
    <header>
      <h1>CAM Importer</h1>
      <nav>
        <NavLink caption="Import" event="navigateToImport" selected={importSelected} />
        <NavLink caption="Assets" event="navigateToAssets" selected={!importSelected} />
      </nav>
    </header>
    {importSelected ? <Import /> : <Assets />}
  </div>);

let NavLink = ({ caption, event, selected }) =>
  selected ?
    <span>{caption}</span> :
    <a href="" onClick={UI.onClick(event)}>{caption}</a>;

let Import = () =>
  <main>
    <Status />
    <Regions />
    <SelectedDealer />
  </main>

let Assets = UI(Web.assets, ({ loaded, data }) =>
  !loaded ? null :
    <main>
      {data.assets.map(asset => <Asset asset={asset} />)}
    </main>);

//
// Status
//

let Status = UI(Web.status, ({ data }) =>
  <section id="status">
    <StartImport />
    <ManifestDownloadError failed={data.manifestDownloadFailed} />
    <ImportWhen caption="last" when={data.whenImportedLast} />
    <ImportWhen caption="next" when={data.whenImportsNext} />
    <Filter />
  </section>);

let StartImport = UI(Queries.startImport, ({ importing, starting, started, startError }) =>
  <button
    id="start-import"
    disabled={importing}
    className={UI.classy({ importing, starting, started, startError })}
    onClick={UI.onClick("startImport")}>

    {importing || starting ? "Importing..." : "Start import"}
  </button>);

let ManifestDownloadError = ({ failed }) =>
  !failed ? null :
    <section className="manifest-download-error">
      The manifest download failed.
      <br />
      <br />
      Try again or contact support.
    </section>

let ImportWhen = ({ caption, when }) => {
  let asMoment = when ? Moment(when) : null;

  return <section className="import-time">
    <h2>{caption}</h2>
    {!asMoment ? null :
      <time dateTime={asMoment.toISOString()}>
        {asMoment.format("ddd MMM D hh:mma")}
      </time>}
  </section>
}

let Filter = UI(Queries.filter, ({ terms }) =>
  <input
    id="filter"
    type="text"
    placeholder="filter dealers..."
    value={terms}
    onChange={UI.onChange("filterChanged")}
  />);

//
// Regions
//

let Regions = UI(Web.regions, ({ loaded }) =>
  !loaded ? null : <FilteredRegions />);

let FilteredRegions = UI(Queries.filteredRegions, ({ matches }) =>
  matches.length === 0 ? null :
    <section id="regions">
      <dl>
        {matches.map(region => <RegionItem {...region} />)}
      </dl>
    </section>);

let RegionItem = ({ name, dealers, isMatch }) =>
  <React.Fragment key={name}>
    <dt className={UI.classy("region", { isMatch })}>{name}</dt>
    {dealers.map(dealer => <DealerItem {...dealer} />)}
  </React.Fragment>

let DealerItem = ({ dealerId, name, isEnrolled, isSelected, isMatch }) =>
  <dd key={dealerId} className={UI.classy("dealer", { isEnrolled, isSelected, isMatch })}>
    <a href="" onClick={UI.onClick("dealerSelected", { dealerId })}>
      {dealerId} {name}
    </a>
  </dd>

//
// Dealer
//

let SelectedDealer = UI(Queries.selectedDealer, ({ detailsLink }) =>
  !detailsLink ? null : <DealerDetails link={detailsLink} />);

let DealerDetails = UI(Web.dealerDetails, ({ loading, loaded, data }) =>
  loading && !loaded ? null :
    <article id="dealer-details">
      <h2>{data.name}</h2>
      <DealerValues data={data} />
      <DealerEnrollment data={data} />
      <DealerErrors data={data} />
    </article>);

let DealerValues = ({ data }) =>
  <section id="dealer-details-values">
    <div className="dealer-details-value">
      <span className="dealer-details-value-caption">Dealer ID:</span>
      <span className="dealer-details-value-data">{data.dealerId}</span>
    </div>

    <div className="dealer-details-value">
      <span className="dealer-details-value-caption">Code:</span>
      <span className="dealer-details-value-data">{data.code}</span>
    </div>

    <div className="dealer-details-value">
      <span className="dealer-details-value-caption">Region:</span>
      <span className="dealer-details-value-data">{data.region}</span>
    </div>

    <div className="dealer-details-value">
      <span className="dealer-details-value-caption">Manifest:</span>
      <a className="dealer-details-value-data" href={data.manifestLink} target="_blank">{data.manifestLink}</a>
    </div>
  </section>

let DealerEnrollment = UI(Queries.changeEnrollment, ({
  dealerId,
  enrolledOnHome,
  enrolledOnConditional,
  saving,
  saved,
  saveError
}) =>
  <section id="dealer-details-enrollment">
    <h3>Enrollment</h3>
    <div id="dealer-details-enrollment-message" className={UI.classy({ saving, saved, saveError })}>
      {saved ? "saved!" : (saveError ? "error" : null)}
    </div>
    <div id="dealer-details-enrollment-body">
      <div className="dealer-details-enrollment-type">
        <label htmlFor="dealer-details-enrollment-home">Home page</label>
        <input
          id="dealer-details-enrollment-home"
          type="checkbox"
          checked={enrolledOnHome}
          onChange={UI.onChange("changeEnrollment", { dealerId, home: !enrolledOnHome, conditional: enrolledOnConditional })}
        />
      </div>

      <div className="dealer-details-enrollment-type">
        <label htmlFor="dealer-details-enrollment-conditional">SRP/VDP</label>
        <input
          id="dealer-details-enrollment-conditional"
          type="checkbox"
          checked={enrolledOnConditional}
          onChange={UI.onChange("changeEnrollment", { dealerId, home: enrolledOnHome, conditional: !enrolledOnConditional })}
        />
      </div>
    </div>
  </section>);

let DealerErrors = ({ data }) =>
  !data.manifestDownloadError && !data.campaignImportError ? null :
    <section id="dealer-details-errors">
      <h3>Errors</h3>
      {!data.manifestDownloadError ? null :
        <section id="dealer-details-manifest-error">
          <h4>Failed to download the manifest</h4>
          <span id="dealer-details-manifest-error-message">
            {data.manifestDownloadError}
          </span>
        </section>}

      {!data.campaignImportError ? null :
        <section id="dealer-details-campaign-error">
          <h4>Failed to import campaigns</h4>
          <span id="dealer-details-campaign-error-message">
            {data.campaignImportError}
          </span>
        </section>}
    </section>

//
// Asset
//

let Asset = ({ asset }) =>
  <div className="asset">
    <img src={asset.cdnLink} alt={asset.altText} />
    {!asset.importFailed ? null :
      <div className="asset-failed">
        ⚠️ The asset download failed. Try importing again or contact support. ⚠️
      </div>}
    <div className="asset-properties">
      <AssetProperty caption="Page(s)" value={getPagesCaption(asset)} />
      <AssetProperty caption="Required" value={asset.required ? "Yes" : "No"} />
      <AssetProperty caption="Site Link" value={asset.siteLink} />
      <AssetProperty caption="Model" value={asset.model} />
      <AssetProperty caption="Model Year" value={asset.modelYear} />
      <AssetProperty caption="Starts" value={getDate(asset.whenStarts)} />
      <AssetProperty caption="Ends" value={getDate(asset.whenEnds)} />
    </div>
  </div>

let AssetProperty = ({ caption, value }) =>
  <div className="asset-property">
    <span className="asset-property-caption">{caption}</span>
    <span className="asset-property-value">{value}</span>
  </div>

function getPagesCaption({ onHome, onSrp, onVdp }) {
  let flags = [onHome, onSrp, onVdp];
  let pages = ["Home", "SRP", "VDP"];

  return pages.filter((_, i) => flags[i]).join(" / ");
};

function getDate(when) {
  return Moment(when).format("YYYY/MM/DD h:mma");
}