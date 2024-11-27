const getWeather = async () => {
  try {
    setError('');
    const response = await axios.get('http://localhost:5000/api/weather', {
      params: { city },
    });
    setWeather(response.data);
  } catch (err) {
    if (err.response && err.response.status === 404) {
      setError('City not found!');
    } else {
      setError('Error fetching weather data');
    }
    setWeather(null);
  }
};
