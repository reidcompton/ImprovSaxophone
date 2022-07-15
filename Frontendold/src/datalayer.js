const datalayer = {
  //serviceUrl: 'https://improvsax20181110122842.azurewebsites.net/api/home',
  serviceUrl: "https://localhost:7209/api/home",
  get() {
    const response = fetch(this.serviceUrl);
    return response;
  },
};

export default datalayer;
