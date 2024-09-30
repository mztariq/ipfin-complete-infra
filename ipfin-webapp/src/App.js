import React, { useState } from 'react';
import axios from 'axios';

function App() {
  const [searchTerm, setSearchTerm] = useState('');
  const [results, setResults] = useState([]);
  const [selectedPostcode, setSelectedPostcode] = useState(null);
  const [postcodeDetails, setPostcodeDetails] = useState(null);
  const maxResults = 10; // Example limit

  // Handle search input change
  const handleSearchChange = (event) => {
    setSearchTerm(event.target.value);
    if (event.target.value.length > 2) {
      searchPostcode(event.target.value);
    } else {
      setResults([]);
    }
  };

  // Search for postcode autocomplete, limiting to maxResults
  const searchPostcode = async (term) => {
    try {
      const response = await axios.get(`http://localhost:5180/autocomplete/${term}`);
      console.log('Autocomplete response:', response.data); // log to see response structure and data
    if (Array.isArray(response.data.result)) {
      setResults(response.data.result.slice(0, 10));
    } else {
      console.error('Unexpected response format:', response.data);
      setResults([]);
    }
    } catch (error) {
      console.error('Error fetching autocomplete results:', error);
    }
  };

  // Get postcode details
  const getPostcodeDetails = async (postcode) => {
    try {
      const response = await axios.get(`http://localhost:5180/lookup/${postcode}`);
      console.log('Autocomplete response for full postcode:', response.data); // log to see response structure and data
      setPostcodeDetails(response.data);
    } catch (error) {
      console.error('Error fetching postcode details:', error);
    }
  };

  // Handle postcode selection
  const handlePostcodeSelect = (postcode) => {
    setSelectedPostcode(postcode);
    getPostcodeDetails(postcode);
  };

  return (
    <div className="App">
      <h1>Postcode Search</h1>
      <input
        type="text"
        value={searchTerm}
        onChange={handleSearchChange}
        placeholder="Enter postcode..."
      />
      <ul>
        {results.length > 0 && results.map((postcode, index) => (
          <li key={index} onClick={() => handlePostcodeSelect(postcode)}>
            {postcode}
          </li>
        ))}
      </ul>

      
        <div>
          <h2>Postcode Details for {selectedPostcode}</h2>
          <p><strong>Country:</strong> {postcodeDetails?.postcodeData?.country}</p>
          <p><strong>Region:</strong> {postcodeDetails?.postcodeData?.region}</p>
          <p><strong>Admin District:</strong> {postcodeDetails?.postcodeData?.codes?.admin_district}</p>
          <p><strong>Parliamentary Constituency:</strong> {postcodeDetails?.postcodeData?.codes?.parliamentary_constituency}</p>
          <p><strong>Area:</strong> {postcodeDetails?.area}</p>
        </div>
      
    </div>
  );
}

export default App;
