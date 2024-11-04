import { useSelector, useDispatch } from 'react-redux';
import { useState, useEffect } from 'react';
import { setMinPrice, setMaxPrice, setSelectedMinPrice, setSelectedMaxPrice } from '../redux/categorySlice';

export const useCategoryPriceRange = () => {
    const dispatch = useDispatch();
    const minPrice = useSelector((state) => state.category.minPrice);
    const maxPrice = useSelector((state) => state.category.maxPrice);
    const selectedMinPrice = useSelector((state) => state.category.selectedMinPrice);
    const selectedMaxPrice = useSelector((state) => state.category.selectedMaxPrice);

    const [localMinPrice, setLocalMinPrice] = useState(selectedMinPrice);
    const [localMaxPrice, setLocalMaxPrice] = useState(selectedMaxPrice);

    useEffect(() => {
        setLocalMinPrice(selectedMinPrice);
        setLocalMaxPrice(selectedMaxPrice);
    }, [selectedMinPrice, selectedMaxPrice]);

    const updatePriceRange = (min, max) => {
        dispatch(setSelectedMinPrice(min));
        dispatch(setSelectedMaxPrice(max));
    };

    return {
        minPrice,
        maxPrice,
        localMinPrice,
        localMaxPrice,
        setLocalMinPrice,
        setLocalMaxPrice,
        updatePriceRange
    };
};