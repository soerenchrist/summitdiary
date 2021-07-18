import axios from 'axios';

const baseUrl = '/api/';

const client = axios.create({
  baseURL: baseUrl,
  json: true,
});

export default {
  async getCountries() {
    const response = await client.get('countries');
    return response.data;
  },
  async getRegions() {
    const response = await client.get('regions');
    return response.data;
  },
  async createSummit(summit) {
    const response = await client.post('summits', summit);
    return response.data;
  },
  async updateSummit(summit) {
    const response = await client.put('summits', summit);
    return response.data;
  },
  async createRegion(region) {
    const response = await client.post('regions', region);
    return response.data;
  },
  async createCountry(country) {
    const response = await client.post('countries', country);
    return response.data;
  },
  async createActivity(activity) {
    const response = await client.post('activities', activity);
    return response.data;
  },
  async updateActivity(activity) {
    const response = await client.put('activities', activity);
    return response.data;
  },
  async getActivity(activityId) {
    const response = await client.get(`activities/${activityId}`);
    return response.data;
  },
  async getSummitImage(summitId) {
    const response = await client.get(`summits/${summitId}/image`);
    return response.data;
  },
  async getActivityPath(activityId) {
    const response = await client.get(`activities/${activityId}/path`);
    return response.data;
  },
  async getSummitById(summitId) {
    const response = await client.get(`summits/${summitId}`);
    return response.data;
  },
  async deleteSummit(summitId) {
    await client.delete(`summits/${summitId}`);
  },
  async deleteActivity(activityId) {
    await client.delete(`activities/${activityId}`);
  },
  async getPagedSummits(options) {
    let {
      page,
      itemsPerPage,
      sortBy,
      sortDesc,
    } = options;

    const { onlyClimbed, bounds } = options;

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

    let url = `summits?pageNumber=${page}&pageSize=${itemsPerPage}&sortBy=${sortBy}&sortDescending=${sortDesc}`;

    if (options.searchText) {
      url += `&searchText=${options.searchText}`;
    }

    if (onlyClimbed) {
      url += '&onlyClimbed=true';
    }

    if (bounds) {
      const {
        swLat,
        swLon,
        neLat,
        neLon,
      } = bounds;
      url += `&swLat=${swLat}&swLon=${swLon}&neLat=${neLat}&neLon=${neLon}`;
    }

    const response = await client.get(url);
    return response.data;
  },
  async getPagedActivities(options) {
    let {
      page,
      itemsPerPage,
      sortBy,
      sortDesc,
    } = options;

    const {
      summitId,
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

    let url = `activities?pageNumber=${page}&pageSize=${itemsPerPage}&sortBy=${sortBy}&sortDescending=${sortDesc}`;

    if (options.searchText) {
      url += `&searchText=${options.searchText}`;
    }

    if (summitId) {
      url = `${url}&summitId=${summitId}`;
    }

    const response = await client.get(url);
    return response.data;
  },
  async uploadGpx(activityId, file) {
    const formData = new FormData();
    formData.append('file', file, file.name);
    formData.append('activityId', activityId);

    const response = await client.post(`activities/${activityId}/gpx`, formData);
    return response.data;
  },
  async analyzeGpx(file) {
    const formData = new FormData();
    formData.append('file', file, file.name);

    const response = await client.post('gpx/analyze', formData);
    return response.data;
  },
  async getTimeline(options) {
    const { valueType, timeType } = options;
    const response = await client.get(`stats/timeline?valueType=${valueType}&timeType=${timeType}`);
    return response.data;
  },
  async getCountryStats(options) {
    const { valueType } = options;
    const response = await client.get(`stats/country?valueType=${valueType}`);
    return response.data;
  },
  async getTotals() {
    const response = await client.get('stats/totals');
    return response.data;
  },
  async getWishlistItemForSummit(summitId) {
    const response = await client.get(`summits/${summitId}/wishlist`);
    return response.data;
  },
  async addSummitToWishlist(summitId) {
    const response = await client.post('wishlist', {
      summitId,
    });

    return response.data;
  },
  async removeFromWishlist(wishlistItemId) {
    await client.delete(`wishlist/${wishlistItemId}`);
  },
  async getWishlist() {
    const response = await client.get('wishlist');
    return response.data;
  },
  async finishWishlistItem(wishlistItem) {
    const response = await client.put(`wishlist/${wishlistItem.id}`);
    return response.data;
  },
};
