import React, {createContext, useEffect, useState} from 'react';
import axios from 'axios';

export const SearchContext = createContext();

export const SearchProvider = ({children}) => {
    const [query, setQuery] = useState('');
    const [results, setResults] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(9);
    const [totalPages, setTotalPages] = useState(0);
    const [sortOrder, setSortOrder] = useState('');

    useEffect(() => {
        const searchParams = new URLSearchParams(window.location.search);
        const queryParam = searchParams.get('query');
        if (queryParam) {
            setQuery(queryParam);
        }
    }, []);

    const fetchProducts = async () => {
        if (query) {
            setIsLoading(true);

            try {
                const response = await axios.get(`/SearchApi`, {
                    params: {
                        query,
                        sortOrder,
                        pageNumber,
                        pageSize,
                    },
                });

                setResults(response.data.products);
                setTotalPages(response.data.totalPages || 1);
            } catch (err) {
                console.error('Error fetching products:', err.message);
                setError(err.message);
            } finally {
                setIsLoading(false);
            }
        }
    };

    useEffect(() => {
        fetchProducts();
    }, [query, sortOrder, pageNumber, pageSize]);

    return (
        <SearchContext.Provider
            value={{
                query,
                setQuery,
                results,
                isLoading,
                error,
                pageNumber,
                setPageNumber,
                pageSize,
                setPageSize,
                totalPages,
                setSortOrder,
                fetchProducts,
            }}
        >
            {children}
        </SearchContext.Provider>
    );
};