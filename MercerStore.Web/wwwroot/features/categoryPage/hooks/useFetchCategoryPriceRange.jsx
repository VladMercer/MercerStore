import {useDispatch} from 'react-redux';
import {useEffect} from 'react';
import {fetchPriceRange, setCategoryId, setSelectedMaxPrice, setSelectedMinPrice} from '../redux/categorySlice';
import {useCategoryPriceRange} from './useCategoryPriceRange';

const getCategoryId = () => {
    const pathParts = window.location.pathname.split('/');
    const categoryId = pathParts[pathParts.length - 1];
    return parseInt(categoryId, 10) || 1;
};

export const useFetchCategoryPriceRange = () => {
    const dispatch = useDispatch();
    const {isPriceRangeLoaded} = useCategoryPriceRange();


    useEffect(() => {
        const categoryId = getCategoryId();
        dispatch(setCategoryId(categoryId));

        if (!isPriceRangeLoaded) {
            dispatch(fetchPriceRange(categoryId));
        }
    }, [isPriceRangeLoaded, dispatch]);

    const updateMinPrice = (min) => {
        dispatch(setSelectedMinPrice(min));

    };

    const updateMaxPrice = (max) => {
        dispatch(setSelectedMaxPrice(max));
    };

    return {updateMinPrice, updateMaxPrice};
};


