import { WetextPage } from './app.po';

describe('wetext App', () => {
  let page: WetextPage;

  beforeEach(() => {
    page = new WetextPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
