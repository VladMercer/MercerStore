import {useSelector} from 'react-redux';

export const useCategoryPriceRange = () => {

    const minPrice = useSelector((state) => state.category.minPrice);
    const maxPrice = useSelector((state) => state.category.maxPrice);
    const selectedMinPrice = useSelector((state) => state.category.selectedMinPrice);
    const selectedMaxPrice = useSelector((state) => state.category.selectedMaxPrice);
    const isPriceRangeLoaded = useSelector((state) => state.category.isPriceRangeLoaded);

    return {
        minPrice,
        maxPrice,
        selectedMinPrice,
        selectedMaxPrice,
        isPriceRangeLoaded
    };
};
