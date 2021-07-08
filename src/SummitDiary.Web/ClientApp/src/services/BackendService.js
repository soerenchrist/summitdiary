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
  async getActivity(activityId) {
    const response = await axios.get(`${baseUrl}activities/${activityId}`);
    return response.data;
  },
  async getSummitImage(summitId) {
    const response = await axios.get(`${baseUrl}summits/${summitId}/image`);
    return response.data;
  },
  async getActivityPath(activityId) {
    const response = await axios.get(`${baseUrl}activities/${activityId}/path`);
    return response.data;
  },
  async getSummitById(summitId) {
    const response = await axios.get(`${baseUrl}summits/${summitId}`);
    return response.data;
  },
  async deleteSummit(summitId) {
    await axios.delete(`${baseUrl}summits/${summitId}`);
  },
  async deleteActivity(activityId) {
    await axios.delete(`${baseUrl}activities/${activityId}`);
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
  async uploadGpx(activityId, file) {
    const formData = new FormData();
    formData.append('file', file, file.name);
    formData.append('activityId', activityId);

    const response = await axios.post(`${baseUrl}activities/${activityId}/gpx`, formData);
    return response.data;
  },
  async analyzeGpx(file) {
    const formData = new FormData();
    formData.append('file', file, file.name);

    const response = await axios.post(`${baseUrl}gpx/analyze`, formData);
    return response.data;
  },
};
