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
};
