import { useState, useEffect } from 'react';
import axios from 'axios';
import SearchBarComponent from './SearchBarComponent';
import { API_SEARCH_URL } from '../../apiConfig';


const SearchBarApp = () => {
    const [query, setQuery] = useState('');
    const [results, setResults] = useState([]);
    const [isDropdownVisible, setDropdownVisible] = useState(false);

    useEffect(() => {
        if (query.length < 2) {
            setResults([]);
            setDropdownVisible(false);
            return;
        }

        const timerId = setTimeout(async () => {
            try {
                const response = await axios.get(`${API_SEARCH_URL}/search/?query=${encodeURIComponent(query)}`);
                setResults(response.data.products);
                setDropdownVisible(true);
            } catch (error) {
                console.error('Error fetching data:', error);
                setResults([]);
            }
        }, 200);

        return () => clearTimeout(timerId);
    }, [query]);

    const handleSubmit = (e) => {
        e.preventDefault();
        window.location.href = `/Search/Index?query=${encodeURIComponent(query)}`;
    };

    return (
        <SearchBarComponent
            query={query}
            setQuery={setQuery}
            results={results}
            isDropdownVisible={isDropdownVisible}
            setDropdownVisible={setDropdownVisible}
            handleSubmit={handleSubmit}
        />
    );
};

export default SearchBarApp;