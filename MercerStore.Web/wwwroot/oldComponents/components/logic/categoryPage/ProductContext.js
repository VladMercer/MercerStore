import React, {createContext, useEffect, useState} from 'react';

export const ProductContext = createContext();

function getQueryParam(param) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(param);
}

export const ProductProvider = ({children}) => {
    const [categoryId, setCategoryId] = useState(getQueryParam('categoryId') || 1);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(9);
    const [sortOrder, setSortOrder] = useState('');
    const [totalProducts, setTotalProducts] = useState(0);
    const [totalPages, setTotalPages] = useState(0);
    const [products, setProducts] = useState([]);

    const [minPrice, setMinPrice] = useState(0);
    const [maxPrice, setMaxPrice] = useState(1000000);
    const [selectedMinPrice, setSelectedMinPrice] = useState(0);
    const [selectedMaxPrice, setSelectedMaxPrice] = useState(1000000);

    const fetchPriceRange = async () => {
        const response = await fetch(`/CategoryApi/price-range?categoryId=${categoryId}`);
        const data = await response.json();
        setMinPrice(data.minPrice);
        setMaxPrice(data.maxPrice);
        setSelectedMinPrice(data.minPrice);
        setSelectedMaxPrice(data.maxPrice);
    };

    const fetchProducts = async (minPrice = selectedMinPrice, maxPrice = selectedMaxPrice) => {
        const response = await fetch(`/CategoryApi/products?categoryId=${categoryId}&sortOrder=${sortOrder}&pageNumber=${pageNumber}&pageSize=${pageSize}&minPrice=${minPrice}&maxPrice=${maxPrice}`);
        const data = await response.json();
        setProducts(data.products);
        setTotalProducts(data.totalItems || 0);
        setTotalPages(data.totalPages || 0);
    };

    useEffect(() => {
        fetchPriceRange();
    }, [categoryId]);


    return (
        <ProductContext.Provider value={{
            categoryId,
            pageNumber,
            pageSize,
            sortOrder,
            totalProducts,
            totalPages,
            products,
            minPrice,
            maxPrice,
            selectedMinPrice,
            selectedMaxPrice,
            setCategoryId,
            setPageNumber,
            setPageSize,
            setSortOrder,
            setSelectedMinPrice,
            setSelectedMaxPrice,
            fetchProducts
        }}>
            {children}
        </ProductContext.Provider>
    );
};