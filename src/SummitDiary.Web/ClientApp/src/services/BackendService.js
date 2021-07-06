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
};
