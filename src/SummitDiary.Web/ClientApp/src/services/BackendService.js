import axios from 'axios';

const baseUrl = '/api/';

export default {
  async getCountries() {
    const response = await axios.get(`${baseUrl}countries`);
    return response.data;
  },
  async getRegions() {
    const response = await axios.get(`${baseUrl}regions`);
    return response.data;
  },
  async createSummit(summit) {
    const response = await axios.post(`${baseUrl}summits`, summit);
    return response.data;
  },
  async createRegion(region) {
    const response = await axios.post(`${baseUrl}regions`, region);
    return response.data;
  },
  async createCountry(country) {
    const response = await axios.post(`${baseUrl}countries`, country);
    return response.data;
  },
  async createActivity(activity) {
    const response = await axios.post(`${baseUrl}activities`, activity);
    return response.data;
  },
  async getSummitById(summitId) {
    const response = await axios.get(`${baseUrl}summits/${summitId}`);
    return response.data;
  },
  async getPagedSummits(options) {
    let {
      page,
      itemsPerPage,
      sortBy,
      sortDesc,
    } = options;

    if (!page) {
      page = 1;
    }

    if (!itemsPerPage) {
      itemsPerPage = 10;
    }

    if (!sortBy || sortBy.length === 0) {
      sortBy = 'name';
    }

    if (!sortDesc || sortDesc.length === 0) {
      sortDesc = false;
    } else {
      [sortDesc] = sortDesc;
    }

    let url = `${baseUrl}summits?pageNumber=${page}&pageSize=${itemsPerPage}&sortBy=${sortBy}&sortDescending=${sortDesc}`;

    if (options.searchText) {
      url += `&searchText=${options.searchText}`;
    }

    const response = await axios.get(url);
    return response.data;
  },
  async getPagedActivities(options) {
    let {
      page,
      itemsPerPage,
      sortBy,
      sortDesc,
    } = options;

    if (!page) {
      page = 1;
    }

    if (!itemsPerPage) {
      itemsPerPage = 10;
    }

    if (!sortBy || sortBy.length === 0) {
      sortBy = 'hikeDate';
    }

    if (!sortDesc) {
      sortDesc = false;
    } else if (sortDesc.length === 0) {
      sortDesc = true;
    } else {
      [sortDesc] = sortDesc;
    }

    let url = `${baseUrl}activities?pageNumber=${page}&pageSize=${itemsPerPage}&sortBy=${sortBy}&sortDescending=${sortDesc}`;

    if (options.searchText) {
      url += `&searchText=${options.searchText}`;
    }

    const response = await axios.get(url);
    return response.data;
  },
};
