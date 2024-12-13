import React from 'react';
import AddToCartButton from '../../cart/components/AddToCartButton';
import { useSearchPage } from '../hooks/useSearchPage';
import useFetchSearchPage from '../hooks/useFetchSearchPage';

const ResultListComponent = () => {
    useFetchSearchPage();
    const { results } = useSearchPage();
   
    return (
        <div className="row">
            {results && results.length > 0 ? (
                results.map(results => (
                    <div className="col-lg-4 col-sm-6 mb-3" key={results.id}>
                        <div className="product-card">
                            <div className="product-thumb">
                                <a href={`/product/details/${results.id}`}>
                                    <img src={results.mainImageUrl} alt="фотокарточка" />
                                </a>
                            </div>
                            <div className="product-details">
                                <h4>
                                    <a
                                        href={`/product/details/${results.id}`}
                                        dangerouslySetInnerHTML={{ __html: results.name }}
                                    ></a>
                                </h4>
                                <p className="product-excerpt">{results.description}</p>
                                <div className="product-bottom-details d-flex justify-content-between">
                                    <div className="product-price">{results.price}₽</div>
                                    <AddToCartButton productId={results.id} />
                                </div>
                            </div>
                        </div>
                    </div>
                ))
            ) : (
                <p>Нет продуктов</p>
            )}
        </div>
    );
};

export default ResultListComponent;