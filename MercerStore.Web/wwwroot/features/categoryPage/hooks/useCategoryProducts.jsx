import {useSelector} from 'react-redux';

export const useCategoryProducts = () => {

    const categoryId = useSelector((state) => state.category.categoryId);
    const products = useSelector((state) => state.category.products);
    const pageNumber = useSelector((state) => state.category.pageNumber);
    const pageSize = useSelector((state) => state.category.pageSize);
    const totalPages = useSelector((state) => state.category.totalPages);
    const sortOrder = useSelector((state) => state.category.sortOrder);
    const totalProducts = useSelector((state) => state.category.totalProducts);
    const isLoaded = useSelector((state) => state.category.isLoaded);
    const isPageReset = useSelector((state) => state.category.isPageReset);

    return {
        products,
        pageNumber,
        pageSize,
        totalPages,
        sortOrder,
        categoryId,
        totalProducts,
        isLoaded,
        isPageReset
    };
};